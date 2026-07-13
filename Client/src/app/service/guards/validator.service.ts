import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidatorService {
    isValidEmail(email: string): boolean {
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        return emailPattern.test(email);
    }

    isValidPassword(password: string): boolean {
        const hasUpperCase = /[A-Z]/.test(password); 
        const hasLowerCase = /[a-z]/.test(password); 
        const hasNumber = /\d/.test(password); 
        const isLongEnough = password.length >= 6; 
    
        return hasUpperCase && hasLowerCase && hasNumber && isLongEnough; 
    }
}