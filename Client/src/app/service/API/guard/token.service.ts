import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
    http = inject(HttpClient)
    constructor() { }

    GetUpdateToken() {
        return this.http.get<{ token: string }>(
            'api/User/UpdateToken',
            {
            withCredentials: true
            }
        );
    }
}