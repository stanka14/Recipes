import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '../services/userservice';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  profileForm: FormGroup;
  passwordForm: FormGroup;
  currentUser: any;
  passwordMessage: string = '';
  message: string = '';
  success: boolean = false;
  passwordSuccess: boolean = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.buildForms();
    this.loadCurrentUser();
  }

  buildForms() {
    this.passwordForm = this.fb.group({
      'currentPassword': ['', [Validators.required, Validators.minLength(8)]],
      'newPassword': ['', [Validators.required, Validators.minLength(8)]],
      'confirmPassword': ['', [Validators.required, Validators.minLength(8)]]
    });

    this.profileForm = this.fb.group({
      'username': ['', { validators: [Validators.required, Validators.minLength(3)], updateOn: 'blur' }],
      'email': ['', [Validators.required, Validators.email]],
      'firstname': ['', Validators.required],
      'lastname': ['', Validators.required]
    });
  }

  onChangePassword() {
    if (this.passwordForm.valid) {
      if (this.passwordForm.value.newPassword === this.passwordForm.value.confirmPassword) {
        const passwordDto = {
          currentPassword: this.passwordForm.value.currentPassword,
          newPassword: this.passwordForm.value.newPassword
        };

        this.userService.changePassword(passwordDto).subscribe({
          next: () => {
            this.passwordMessage = "Password changed successfully.";
            this.passwordSuccess = true;
            
            this.passwordForm.reset();
            this.passwordForm.get('currentPassword').setErrors(null);
            this.passwordForm.get('newPassword').setErrors(null);
            this.passwordForm.get('confirmPassword').setErrors(null);

            this.alertError('Password changed successfully');
          },
          error: (error) => {
            this.processError(error);
          }
        });
      } else {
        this.alertError("New password and confirm password do not match.");
      }
    } else {
      this.passwordForm.markAllAsTouched();
    }
  }

  loadCurrentUser() {
    this.userService.loadCurrentUser().subscribe({
      next: (data) => {
        this.currentUser = data;
        this.profileForm.patchValue({
          'username': this.currentUser.username,
          'email': this.currentUser.email,
          'firstname': this.currentUser.firstname,
          'lastname': this.currentUser.lastname,
        });
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  onSubmit() {
    if (this.profileForm.valid) {
      const userDto = {
        id: this.currentUser.id,
        username: this.profileForm.value.username,
        email: this.profileForm.value.email,
        password: this.profileForm.value.password,
        firstname: this.profileForm.value.firstname,
        lastname: this.profileForm.value.lastname,
      };

      this.userService.updateUser(userDto).subscribe({
        next: () => {
          this.success = true;
          this.message = "Profile changes saved successfully";
          this.alertError('Profile changes saved successfully');
        },
        error: (error) => {
          this.processError(error);
        }
      });
    } else {
      this.profileForm.markAllAsTouched();
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
