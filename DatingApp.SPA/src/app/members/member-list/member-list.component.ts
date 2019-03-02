import { Component, OnInit } from '@angular/core';
import { UserService } from '../../sevices/user.service';
import { AlertifyService } from '../../sevices/alertify.service';
import { User } from '../../models/User';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];

  constructor(private _userService: UserService, private _alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this._userService.getUsers()
      .subscribe((users: User[]) => {
        this.users = users;
      }, error => {
        this._alertify.error(error);
      });
  }
}
