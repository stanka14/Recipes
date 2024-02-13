import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/userservice';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  errorMessage: string | null = null;
  formErrors = {
    'email': '',
    'password': '',
    'firstname': '', 
    'lastname': '',
    'username': ''

  };
  validationMessages = {
    'email': {
      'required': 'Please enter your email',
      'email': 'please enter your vaild email'
    },
    'password': {
      'required': ' Please enter your password',
      'minlength': 'Please enter more than 8 characters',
    },
    'lastname': {
      'required': ' Please enter your last name',
    },
    'firstname': {
      'required': ' Please enter your first name',
    },
    'username': {
      'required': ' Please enter your  username',
      'minlength': 'Please enter more than 3 characters',
    }
  };

  registrationForm : FormGroup = new FormGroup({});

  constructor(private router: Router, private userservice: UserService, private fb: FormBuilder, private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.registrationForm = this.fb.group({
      'username': ['', { validators: [Validators.required, Validators.minLength(3)], updateOn: 'blur' }],
      'email': ['', [Validators.required, Validators.email]],
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'password': ['', { validators: [Validators.required, Validators.minLength(8)], updateOn: 'blur' }],
    });
  }

  register() {
    if (this.registrationForm.valid) {
      const userDto = {
        username: this.registrationForm.value.username,
        firstname: this.registrationForm.value.firstname,
        lastname: this.registrationForm.value.lastname,
        password: this.registrationForm.value.password,
        email: this.registrationForm.value.email,
        id: ''
      }
      this.userservice.register(userDto).subscribe({
        next: () => {
          this.alertError('Success! Please log in.');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.processError(error);
        }
      });
    }
    else {
      this.registrationForm.markAllAsTouched();
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
