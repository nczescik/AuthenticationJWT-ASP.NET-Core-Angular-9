import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  constructor(private fb: FormBuilder, public service: UserService) {
    this.loginForm = this.fb.group({
      'UserName': ['', Validators.required],
      'Password': ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  login() {
    this.service.login(this.loginForm.value).subscribe();
    this.loginForm.reset();
  }

  get username(){
    return this.loginForm.get('UserName');
  }

  get password(){
    return this.loginForm.get('Password');
  }
}
