import {Component, HostListener, OnInit, ViewChild} from '@angular/core';
import {Member} from "../../_models/member";
import {IUser} from "../../_models/user";
import {AccountService} from "../../_services/account.service";
import {MembersService} from "../../_services/members.service";
import {take} from "rxjs";
import {CommonModule} from "@angular/common";
import {GalleryComponent} from "ng-gallery";
import {TabsModule} from "ngx-bootstrap/tabs";
import {FormsModule, NgForm} from "@angular/forms";
import {ToastrService} from "ngx-toastr";
import { NgxSpinnerModule } from "ngx-spinner";

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [CommonModule, GalleryComponent, TabsModule, FormsModule, NgxSpinnerModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements  OnInit{
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any){
    $event.returnValue=true;
  }
  member: Member | undefined;
  user: IUser | null = null;
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUsers.pipe(take(1)).subscribe(
      user => {
        this.user = user
      }
    )
  }
  ngOnInit() {
    this.loadMember()
  }

  loadMember(){
    if(!this.user) return;
    this.memberService.getMember(this.user.username).subscribe(member =>{
      this.member=member
    })
  }

  updateMember(){
    this.memberService.updateMember(this.editForm?.value).subscribe( _ => {
      this.toastr.success('Profile updated successfully');
      this.editForm?.reset(this.member);
    })

  }
}
