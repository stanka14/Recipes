import { Component, OnInit, Input } from '@angular/core';
import { ToolbarHelpers } from './toolbar.helpers';
import { UserService } from '../../services/userservice';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'cdk-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
	currentUser: any;
    @Input() sidenav;
	@Input() sidebar;
	@Input() drawer;
	@Input() matDrawerShow;
  
	searchOpen: boolean = false;
    toolbarHelpers = ToolbarHelpers;
  	constructor(private userservice: UserService, private snackBar: MatSnackBar) { }

  	ngOnInit() {

		this.currentUser = this.loadCurrentUser();
	}
	loadCurrentUser() {

		  this.userservice.loadCurrentUser().subscribe({
			next: (data) => {
			  this.currentUser = data;	
			},
			error: (error) => {
				this.processError(error);
			}
		  });
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
	
