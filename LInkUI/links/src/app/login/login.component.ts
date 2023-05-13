import { Component, OnInit } from '@angular/core';
import { FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { LinksService } from '../services/links.service';
import { LoginService } from '../services/login.service';
import { UserDto } from '../models/userDto.model';
import { DialogRef } from '@angular/cdk/dialog';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  hide = true;
  user!: UserDto;
  isLogin: boolean = false;

  constructor(
    private _loginService: LoginService,
    private _dgRef: DialogRef<LoginComponent>,
    private router: Router,
  ) {
    this.isLogin = _loginService.isloggedin();
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(4)])
    });
  }

  onLogin() {
    if (!this.loginForm.valid) {
      return;
    }
    console.log(this.loginForm.value.username);
    this._loginService.userAuthentication(this.loginForm.value.username, this.loginForm.value.password).
      subscribe({
        next: (data: any) => {
          localStorage.setItem('userToken', data.token);
          this._dgRef.close();
          this.router.navigate(['/']);
          window.location.reload();
        },
        error: (err: any) => {
          alert(err.error);
          console.log(err);
        }
      });
  }
}
