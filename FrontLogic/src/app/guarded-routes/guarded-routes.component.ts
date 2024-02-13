
import { Component, OnInit } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../services/authservice';

@Component({
  selector: 'app-guarded-routes',
  templateUrl: './guarded-routes.component.html',
  styleUrls: ['./guarded-routes.component.scss']
})
export class GuardedRoutesComponent implements CanActivate {
  message = 'You do not have permission to access this link';
  action = 'Exit';
  constructor(public router: Router, public snackBar: MatSnackBar, private authservice: AuthService) {

  }

  ngOnInit() {

  }
    canActivate(): boolean {
    const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
  
    if (isLoggedIn) {
    {
      if(this.isSessionExpired())
      {
        this.authservice.logout();
        return false;
      }
      return true;
    }
    } else {
      this.snackBar.open(this.message, this.action, {
        duration: 2000,
      });
      this.router.navigate(['/login']);
      return false;
    }
  }    
   isSessionExpired() {
    const token = localStorage.getItem('token');
    if (!token) {
      return true;
    }
  
    const tokenData = this.parseJwt(token); 
  
    if (!tokenData || !tokenData.exp) {
      return true;
    }
  
    const expirationTimestamp = tokenData.exp * 1000; 
    const currentTimestamp = Date.now(); 
  
    return currentTimestamp > expirationTimestamp;
  }
  
   parseJwt(token: any) {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
      return null;
    }
  }

}
