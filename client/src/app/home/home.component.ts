import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment.development";
import {take} from "rxjs";

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [CommonModule, RegisterComponent]
})
export class HomeComponent implements OnInit{

  registerMode = false;
  homeData: any = {
    title : '',
    description : ''
  };

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.httpClient.get<any>(environment.apiUrl + "Home").pipe(take(1)).subscribe(response => {
      this.homeData = response;
    });
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event : boolean){
    this.registerMode = event;
  }
}
