import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
    http = inject(HttpClient)
    constructor() { }

    GetLogout() {
        return this.http.get<{ token: string }>(`api/User/Logout`, {
            withCredentials: true
        });
    }
}