import { Component, OnInit, Input, HostListener, ElementRef } from '@angular/core';
import { AuthService } from '../../services/authservice';
import { Router } from '@angular/router';

@Component({
  selector: 'cdk-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.scss']
})
export class UserMenuComponent implements OnInit {
	isOpen: boolean = false;

  	@Input() currentUser: any;
  	@HostListener('document:click', ['$event', '$event.target'])
  	onClick(event: MouseEvent, targetElement: HTMLElement) {
    	if (!targetElement) {
     		return;
    	}

    	const clickedInside = this.elementRef.nativeElement.contains(targetElement);
    	if (!clickedInside) {
      		this.isOpen = false;
    	}
  	}
  	
    
  	constructor(private router: Router, private elementRef: ElementRef, private authservice: AuthService) { }


  	ngOnInit() {
  	}

	logout(): void {
		this.authservice.logout();
		this.router.navigate(['/login']);
	}

	editProfile(): void {
		this.isOpen = false;
		this.router.navigate(['/auth/user-details']);
	  }	  
}
