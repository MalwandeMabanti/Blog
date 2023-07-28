import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  user = {
    
  };

  constructor(private authService: AuthService) { }

  register(): void {
    this.authService.registerUser(this.user)
      .subscribe(
        response => console.log('User registered', response),
        error => console.log('Error registering user', error)
      );
  }

}
