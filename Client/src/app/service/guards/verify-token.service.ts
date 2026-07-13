import { Injectable, inject } from '@angular/core';
import { catchError, interval, map, Observable, of, Subscription } from 'rxjs';
import { TokenService } from '../API/guard/token.service';
import { CookieService } from 'ngx-cookie-service';
import {Router} from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class VerifyTokenService {
    private excludedRoutes = [
        '/login',
        '/register'
    ]
    private authService = inject(TokenService)
    private timer?: Subscription

    constructor(private router: Router, private cookieService: CookieService) { }

    start() {
        if (!this.cookieService.check('access_token')) return;

        if (!this.timer || this.timer.closed) {
            this.timer = interval(3000).subscribe(() => {
                const url = this.router.url;
                if (this.excludedRoutes.some(route => url.startsWith(route))) {
                    return;
                }
                this.authService.GetUpdateToken().subscribe({
                    next: (res) => {
                        this.cookieService.set('access_token', res.token, 1 / 24, '/');
                    },
                    error: () => {
                        this.router.navigate(['login']);
                    }
                });
            });
        }
    }

    canActivate(): Observable<boolean> {
        if (!this.cookieService.check('access_token')) {
            this.router.navigate(['login']);
            return of(false);
        }

        return this.authService.GetUpdateToken().pipe(
            map(res => {
                this.cookieService.set('access_token', res.token, 1 / 24, '/');
                return true;
            }),
            catchError(() => {
                this.router.navigate(['login']);
                return of(false);
            })
        );
    }
}