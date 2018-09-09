import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../sevices/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  @Output() onCancelRegister = new EventEmitter();

  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model)
                    .subscribe(() => {
                      console.log('register ok');
                    }, error => {
                      console.log(error);
                    });
  }

  cancel() {
    this.onCancelRegister.emit(false);
    console.log('cancelled');
  }
}
