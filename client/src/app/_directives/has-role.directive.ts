import {Directive, Input, OnInit, TemplateRef, ViewContainerRef} from '@angular/core';
import {IUser} from "../_models/user";
import {AccountService} from "../_services/account.service";
import {take} from "rxjs";

@Directive({
  selector: '[appHasRole]',
  standalone: true
})
export class HasRoleDirective implements OnInit{
  @Input() appHasRole: string[] = [];
  user: IUser = {} as IUser;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, private accountService: AccountService) {
    this.accountService.currentUsers.pipe(take(1)).subscribe( user => {
      if(user) this.user = user;
    })
  }

  ngOnInit(): void {
    if (this.user.roles.some(r => this.appHasRole.includes(r))){
      this.viewContainerRef.createEmbeddedView((this.templateRef));
    }
    else {
      this.viewContainerRef.clear();
    }
  }

}
