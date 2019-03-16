import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from '../../models/User';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AlertifyService } from '../../sevices/alertify.service';
import { AuthService } from '../../sevices/auth.service';
import { UserService } from '../../sevices/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;

  user: User;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private alertify: AlertifyService,
    private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  updateData() {
    this.userService
      .updateUser(this.authService.decodedToken.nameid, this.user)
      .subscribe(() => {
        this.alertify.success('Profile updated successfuly!');
        this.editForm.reset(this.user);
      }, (error: string) => {
        this.alertify.error(error);
      });
  }
}
