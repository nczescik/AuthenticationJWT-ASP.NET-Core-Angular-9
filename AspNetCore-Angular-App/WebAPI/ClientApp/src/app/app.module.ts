import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { AuthenticationComponent } from './users/authentication/authentication.component';
import { RegistrationComponent } from './users/authentication/registration/registration.component';
import { AuthService } from './services/Authentication/auth.service';
import { LoginComponent } from './users/authentication/login/login.component';
import { TokenInterceptorSevice } from './services/Authentication/token-interceptor.service';
import { ErrorInterceptorService } from './services/Errors/error-interceptor.service';
import { MiniProfilerInterceptor, MiniProfilerModule } from 'ng-miniprofiler';

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
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    MiniProfilerModule.forRoot({
      baseUri: 'https://localhost:44343',
      colorScheme: 'Auto',
      maxTraces: 15,
      position: 'BottomLeft',
      toggleShortcut: 'Alt+M',
      enabled: true,
      enableGlobalMethod: true
    }),
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorSevice,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MiniProfilerInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
