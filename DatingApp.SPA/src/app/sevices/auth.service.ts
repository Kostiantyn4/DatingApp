import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';
  userToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {

    return this.http.post(this.baseUrl + 'login', model, this.httpOptions())
                    .pipe(map((response: any) => {
                      const token = response.token;
                      if (token) {
                        localStorage.setItem('token', token);
                        this.userToken = token;
                      }
                    }));
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model, this.httpOptions());
  }

  private httpOptions() {
    return {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
  }
}
