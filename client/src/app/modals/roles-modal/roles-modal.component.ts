import {Component, OnInit} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {BsModalRef} from "ngx-bootstrap/modal";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-roles-modal',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    FormsModule
  ],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css'
})
export class RolesModalComponent implements OnInit{
  username = '';
  availableRoles: any[] = [];
  selectedRoles: any[] = [];
  constructor(public bsModalRef: BsModalRef) {
  }
  ngOnInit(): void {
  }

  updateChecked(checkedValue: string){
    const index = this.selectedRoles.indexOf(checkedValue);
    index !== -1 ? this.selectedRoles.splice(index, 1) : this.selectedRoles.push(checkedValue);
  }

}
