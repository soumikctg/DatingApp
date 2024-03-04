import {Component, OnInit, ViewChild} from '@angular/core';
import {Member} from "../../_models/member";
import {MembersService} from "../../_services/members.service";
import {ActivatedRoute} from "@angular/router";
import {CommonModule} from "@angular/common";
import {TabDirective, TabsetComponent, TabsModule} from "ngx-bootstrap/tabs";
import {GalleryItem, GalleryModule, ImageItem} from "ng-gallery";
import {TimeagoModule} from "ngx-timeago";
import {MemberMessagesComponent} from "../member-messages/member-messages.component";
import {MessageService} from "../../_services/message.service";
import {Message} from "../../_models/message";


@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit{
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  activateTab? : TabDirective;
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  messages: Message[] = [];
  constructor(private memberService : MembersService, private route: ActivatedRoute, private messageService: MessageService) {
  }
  ngOnInit() {
    this.route.data.subscribe(
      data => this.member = data['member']
    )
    this.route.queryParams.subscribe(
      params => params['tab'] && this.selectTab(params['tab'])
    )
    this.getImages()
  }

  selectTab(heading: string){
    if(this.memberTabs){
      this.memberTabs.tabs.find(x => x.heading == heading)!.active = true;
     }
  }
  onTabActivated(data: TabDirective){
    this.activateTab = data;
    if(this.activateTab.heading === 'Messages'){
      this.loadMessages();
    }
  }

  loadMessages(){
    if(this.member){
      this.messageService.getMessageThread(this.member.userName).subscribe(messages => this.messages = messages)
    }
  }

  getImages(){
    if(!this.member) return;
    for(const photo of this.member?.photos){
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }


}
