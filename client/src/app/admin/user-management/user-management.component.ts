import {Component, OnInit} from '@angular/core';
import {AdminService} from "../../_services/admin.service";
import {IUser} from "../../_models/user";
import {NgForOf} from "@angular/common";
import {BsModalRef, BsModalService, ModalModule, ModalOptions} from "ngx-bootstrap/modal";
import {RolesModalComponent} from "../../modals/roles-modal/roles-modal.component";

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    NgForOf
  ],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit{
  users: IUser[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  availableRoles = [
    'Admin',
    'Moderator',
    'Member',
  ]
  constructor(private adminService: AdminService, private modalService: BsModalService) {
  }
  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe(users => this.users = users);
  }

  openRolesModal(user: IUser){

    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        username: user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.onHide?.subscribe( () => {
      const selectedRoles = this.bsModalRef.content?.selectedRoles;
      if(!this.arrayEqual(selectedRoles!, user.roles)){
        this.adminService.updateUserRoles(user.username, selectedRoles!).subscribe( roles => user.roles = roles )
      }
    })
  }


  private arrayEqual(arr1: any[], arr2: any[]) {
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }

}
