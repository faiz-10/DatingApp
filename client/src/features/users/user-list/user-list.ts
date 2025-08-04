import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Observable, Observer } from 'rxjs';
import { Member } from '../../../types/member';
import { MemberService } from '../../../core/services/member-service';
import { MemberCard } from "../../members/member-card/member-card";

@Component({
  selector: 'app-user-list',
  imports: [AsyncPipe, MemberCard],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList {
  private memberService = inject(MemberService);
  protected members$: Observable<Member[]>;

  constructor() {
    this.members$ = this.memberService.getMembers();
  }
}
