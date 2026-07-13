import { Component, inject } from '@angular/core';
import { UserService } from '../../service/API/user/user.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { HistoryService } from '../../service/history/history.service';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  Rest = inject(UserService);

  menu_open: boolean = false;
  profile_menu_open: boolean = false;

  constructor(private router: Router, private cookieService: CookieService, private historyService: HistoryService) { }

  toggleMenu() {
    this.menu_open = !this.menu_open;
  }

  
  toggle_profile_Menu() {
    this.profile_menu_open = !this.profile_menu_open;
  }

  Logout() {
    this.Rest.GetLogout().subscribe({
      next: (value) => {
        this.cookieService.delete("access_token");
        this.cookieService.delete("refresh_token");

        this.historyService.ClearSearchHistory();

        this.router.navigate(['Login'])
      },
    })
  }
}
