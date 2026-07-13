import { Routes } from '@angular/router';
import { Register } from './pages/auth/register/register';
import { Login } from './pages/auth/login/login';
import { Home } from './pages/home/home';
import { CardItem } from './pages/card-item/card-item';
import { VerifyTokenService } from './service/guards/verify-token.service';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'register', component: Register },
    { path: 'login', component: Login },
    { path: 'home', component: Home, canActivate: [VerifyTokenService] },
    { path: 'c/:CardItemId', component: CardItem, canActivate: [VerifyTokenService]},
];
