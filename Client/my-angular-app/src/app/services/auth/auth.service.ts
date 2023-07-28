import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Replace with the URL of your API
  private apiURL = 'http://localhost:3000/api';

  constructor(private http: HttpClient) { }

  registerUser(user: {}): Observable<any> {
    return this.http.post<any>(`${this.apiURL}/register`, user);
  }

  loginUser(user: {}): Observable<any> {
    return this.http.post<any>(`${this.apiURL}/login`, user);
  }
}
