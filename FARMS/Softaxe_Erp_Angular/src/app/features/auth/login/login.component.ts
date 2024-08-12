import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  userName: string = '';
  password: string = '';

  loginForm!: FormGroup;
  isLoginPage: boolean = false;
  passType: string = 'password';
  eye: string = 'fa-eye-slash';
  isPassword: boolean = true;

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastrService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });

    this.isLoginPage = this.route.snapshot.data['isLoginPage'];
  }

  onClickLogin() {
    if (this.loginForm.valid) {
      this.authService.onLogin(this.loginForm.value).subscribe({
        next: (res) => {
          this.authService.storeToken(res);
          this.toast.success(res.msg);
          this.loginForm.reset();
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.toast.error(err?.error.msg);
        },
      });
    } else {
      this.validateAllFormFields(this.loginForm);
    }
  }

  onClickEye() {
    this.isPassword = !this.isPassword;
    if (!this.isPassword) {
      this.passType = 'text';
      this.eye = 'fa-eye';
    } else {
      this.passType = 'password';
      this.eye = 'fa-eye-slash';
    }
  }

  private validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach((field) => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control?.markAsDirty({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }
}
