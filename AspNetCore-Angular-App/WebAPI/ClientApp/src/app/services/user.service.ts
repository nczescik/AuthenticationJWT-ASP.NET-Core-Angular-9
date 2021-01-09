import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }
  readonly BaseUrl = environment.baseUrl;

  register(data: any): Observable<any> {
    return this.http.post(this.BaseUrl + '/Users/Register', data);
  }

  login(data: any): Observable<any> {
    return this.http.post(this.BaseUrl + '/Users/Login', data);
  }
}
