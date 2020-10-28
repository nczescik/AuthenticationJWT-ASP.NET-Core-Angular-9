import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http'
import { FormsModule }   from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AuthenticationComponent } from './users/authentication/authentication.component';
import { RegistrationComponent } from './users/authentication/registration/registration.component';
import { UserService } from './services/user.service';
import { LoginComponent } from './users/authentication/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthenticationComponent,
    RegistrationComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
