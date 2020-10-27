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
    FirstName: [''],
    LastName: [''],
    UserName: [''],
    Password: ['']
  })

  register() {
    var body = {
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      UserName: this.formModel.value.UserName,
      Password: this.formModel.value.Password,
    }

    return this.http.post(this.BaseUrl + '/Users/Register', body).subscribe();
  }
}
