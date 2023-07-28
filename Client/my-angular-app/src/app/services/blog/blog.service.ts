import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BlogService {
  private apiUrl = 'http://localhost:7010/api'; // replace with your API endpoint

  constructor(private http: HttpClient) { }

  getBlogs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/blogs`);
  }

  createBlog(blogData: { title?: string | null, content?: string | null, image?: string | null }): Observable<any> {
    return this.http.post(`${this.apiUrl}/blogs`, blogData);
  }
}
