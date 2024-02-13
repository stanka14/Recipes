import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/authservice';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  userForm: FormGroup = new FormGroup({});
  formErrors = {
    'email': '',
    'password': ''
  };

  validationMessages = {
    'email': {
      'required': 'Please enter your email',
      'email': 'Please enter your vaild email'
    },
    'password': {
      'required': 'Please enter your password!'
    }
  };

  constructor(private router: Router,
              private fb: FormBuilder, private authservice: AuthService, private snackBar: MatSnackBar) {
              }

  ngOnInit() {d
    this.buildForm();
    localStorage.removeItem('token');
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('currentUser');
    this.authservice.logout();
  }

  buildForm() {
    this.userForm = this.fb.group({
      'email': ['', [
        Validators.required,
        Validators.email
      ]
      ],
      'password': ['', [
        Validators.required,
      ]
      ],
    });
  }

  login() {
    this.authservice.logout();
    if (this.userForm.valid) {
      
      this.authservice.login(this.userForm.value).subscribe(
        (response) => {
          localStorage.setItem('token', response.token);
  
          const userObject = {
            id: response.id,
            username: response.username,
            firstname: response.firstname,
            lastname: response.lastname,
            email: response.email
          };
                  
          localStorage.setItem('isLoggedIn', 'true');
          this.router.navigate(['/dashboard']);
        },
        (error) => {
          this.processError(error);
        }
      );
    }
    else
    {
      this.userForm.markAllAsTouched();
    }
  }
  
  processError(error)
  {
    if(error.error.errors){
      var errors = error.error.errors;
      Object.keys(errors).forEach(key => {
        this.alertError(`Key: ${key}, Value: ${errors[key][0]}`);
      });   
    }
    else
    {
      this.alertError(error.error);
    }   
  }

  alertError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
    console.error(message);
  }
}

