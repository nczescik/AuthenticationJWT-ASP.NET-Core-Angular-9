import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public service: UserService) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.service.login();
    this.service.formModel.reset();
  }
}
