import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  user = {

  };

  constructor(private authService: AuthService) { }

  login(): void {
    this.authService.loginUser(this.user)
      .subscribe(
        response => console.log('User logged in', response),
        error => console.log('Error logging in', error)
      );
  }

}
