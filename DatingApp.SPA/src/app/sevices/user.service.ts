import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/User';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.baseUrl}users`);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${environment.baseUrl}users/${id}`);
  }

  updateUser(id: number, user: User) {
    return this.http.put(`${environment.baseUrl}users/${id}`, user);
  }

  setMainPhoto(userId: number, photoId: number) {
    return this.http.post(`${environment.baseUrl}users/${userId}/photos/${photoId}/setMain`, {});
  }
}
