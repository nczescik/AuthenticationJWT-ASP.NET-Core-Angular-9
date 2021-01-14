import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/Authentication/auth.service';
import { ValidationService } from 'src/app/services/Validation/validation.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private validationService: ValidationService) {
    this.loginForm = this.fb.group({
      'UserName': ['', Validators.required],
      'Password': ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  login() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe(data => {
        this.authService.saveToken(data.token);
      });
      this.loginForm.reset();
    } else {
      this.validationService.validateAllFormFields(this.loginForm);
    }
  }

  get username() {
    return this.loginForm.get('UserName');
  }

  get password() {
    return this.loginForm.get('Password');
  }
}
