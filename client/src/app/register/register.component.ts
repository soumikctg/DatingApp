import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{

  model: any = {};

  constructor(){

  }

  ngOnInit(): void {
    
  }

  register(){
    console.log(this.model);
  }
  cancel(){
    console.log("Cancelled");
  }
}