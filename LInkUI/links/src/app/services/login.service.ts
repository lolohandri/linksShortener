import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  baseUrl: string = "https://localhost:7076/api/auth"

  constructor(private _http: HttpClient) { }

  userAuthentication(username: string, password: string): Observable<any> {
    let data = {
      username: username,
      password: password,
    };
    return this._http.post<string>(this.baseUrl + '/login', data);
  }

  isloggedin() {
    return localStorage.getItem('userToken') != null;
  }
  getToken() {
    return localStorage.getItem('userToken');
  }
}
