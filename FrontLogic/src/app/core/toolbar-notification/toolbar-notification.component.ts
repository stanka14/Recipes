import { Component, OnInit, ElementRef, HostListener, OnDestroy } from '@angular/core';
import { Notification } from '../../models/notification';
import { UserService } from '../../services/userservice';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notifications',
  templateUrl: './toolbar-notification.component.html',
  styleUrls: ['./toolbar-notification.component.scss']
})

export class NotificationsComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  isOpen = false;
  notificationInterval: any;
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
  	
  constructor(private userService: UserService, private elementRef: ElementRef, private snackBar: MatSnackBar, private router: Router) {}

  ngOnInit() {
    this.notificationInterval = setInterval(() => {
      this.loadNotifications();
    }, 10000);
  }

  ngOnDestroy() {
    clearInterval(this.notificationInterval);
  }

  openRecipe(notification: Notification) {
    if (notification.notificationType == "Comment" || notification.notificationType == "Rate") {
        this.userService.makeAsRead(notification.id).subscribe(
          () => {
            //this.router.navigate(['/auth/recipe-details', notification.relatedObjectId]);
            this.router.navigate(['/auth/recipe-details', notification.relatedObjectId], { queryParams: { refresh: true } });
            this.loadNotifications();
          },
          (error) => {
            this.processError(error);
          }
        );      
    }
  }

  toggleNotifications() {
    this.isOpen = !this.isOpen;
  }

  markAllAsRead() {
    this.userService.loadCurrentUser().subscribe({
      next: (user) => {
        const userId = user.id;
        this.userService.makeAllAsRead(userId).subscribe(() => {
          this.loadNotifications();
          this.alertError("Success!");
        });
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  deleteNotification(notification: Notification) {
      this.userService.loadCurrentUser().subscribe({
        next: (user) => {
          const userId = user.id;
  
          this.userService.deleteNotification(notification.id).subscribe(() => {
            this.loadNotifications();
          });
        },
        error: (error) => {
          this.processError(error);
        }
      });
  }

  private loadNotifications() {
    this.userService.loadCurrentUser().subscribe({
      next: (user) => {
        const userId = user.id;

        this.userService.getAllNotifications(userId).subscribe((notifications) => {
          this.notifications = notifications;
        });
      },
      error: (error) => {
        this.processError(error);
      }
    });
  }

  getUnreadNotificationCount(): number {
    return this.notifications.filter(notification => !notification.isRead).length;
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
