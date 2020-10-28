import { Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }
  readonly BaseUrl = environment.baseUrl;

  formModel = this.fb.group({
    UserName: '',
    Password: '',
    ConfirmPassword: ''
  })

  register() {
    var body = {
      UserName: this.formModel.value.UserName,
      Password: this.formModel.value.Password,
      ConfirmPassword: this.formModel.value.ConfirmPassword
    }
    return this.http.post(this.BaseUrl + '/Users/Register', body).subscribe();
  }

  login() {
    var body = {
      UserName: this.formModel.value.UserName,
      Password: this.formModel.value.Password
    }
    return this.http.post(this.BaseUrl + '/Users/Login', body).subscribe();
  }
}
