import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/Authentication/auth.service';
import { ValidationService } from 'src/app/services/Validation/validation.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registerForm: FormGroup;
  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private validationService: ValidationService) {
    this.registerForm = this.fb.group({
      'UserName': ['', Validators.required],
      'Password': ['', Validators.required],
      'ConfirmPassword': ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  register() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe();
      this.registerForm.reset();
    } else {
      this.validationService.validateAllFormFields(this.registerForm);
    }
  }

  get username() {
    return this.registerForm.get('UserName');
  }

  get password() {
    return this.registerForm.get('Password');
  }

  get confirmPassword() {
    return this.registerForm.get('ConfirmPassword');
  }

}
