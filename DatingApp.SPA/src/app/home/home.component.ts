import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isRegisterMode = false;
  values: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {

  }

  registerToggle() {
    this.isRegisterMode = !this.isRegisterMode;
  }

  onCancelRegister(isRegisterMode: boolean) {
    this.isRegisterMode = isRegisterMode;
  }
}
