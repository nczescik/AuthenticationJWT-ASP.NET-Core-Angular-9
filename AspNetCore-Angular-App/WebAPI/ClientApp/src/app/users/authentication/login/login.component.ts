import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  constructor(private fb: FormBuilder, public authService: AuthService) {
    this.loginForm = this.fb.group({
      'UserName': ['', Validators.required],
      'Password': ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  login() {
    this.authService.login(this.loginForm.value).subscribe(data => {
      this.authService.saveToken(data.token);
    });
    this.loginForm.reset();
  }

  get username(){
    return this.loginForm.get('UserName');
  }

  get password(){
    return this.loginForm.get('Password');
  }
}
