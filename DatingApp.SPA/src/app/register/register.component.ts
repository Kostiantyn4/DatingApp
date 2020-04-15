import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../sevices/auth.service';
import { AlertifyService } from '../sevices/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() onCancelRegister = new EventEmitter();

  model: any = {};

  constructor(private authService: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService
      .register(this.model)
      .subscribe(() => {
        this.alertify.success('Register ok');
      }, error => {
        this.alertify.error(error);
      });
  }

  cancel() {
    this.onCancelRegister.emit(false);
  }
}
