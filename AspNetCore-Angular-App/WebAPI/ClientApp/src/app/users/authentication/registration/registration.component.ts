import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registerForm: FormGroup;
  constructor(private fb: FormBuilder, public authService: AuthService) { 
    this.registerForm = this.fb.group({
      'UserName': ['', Validators.required],
      'Password': ['', Validators.required],
      'ConfirmPassword': ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  register() {
    this.authService.register(this.registerForm.value).subscribe();
    this.registerForm.reset();
  }

  get username(){
    return this.registerForm.get('UserName');
  }

  get password(){
    return this.registerForm.get('Password');
  }

  get confirmPassword(){
    return this.registerForm.get('ConfirmPassword');
  }

}
