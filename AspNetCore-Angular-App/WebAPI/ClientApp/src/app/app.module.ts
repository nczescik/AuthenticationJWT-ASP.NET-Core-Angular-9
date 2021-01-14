import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { FormsModule }   from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AuthenticationComponent } from './users/authentication/authentication.component';
import { RegistrationComponent } from './users/authentication/registration/registration.component';
import { AuthService } from './services/Authentication/auth.service';
import { LoginComponent } from './users/authentication/login/login.component';
import { TokenInterceptorSevice } from './services/Authentication/token-interceptor-sevice.service';

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
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorSevice,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
