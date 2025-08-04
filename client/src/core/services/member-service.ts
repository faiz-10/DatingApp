import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Member, Photo } from '../../types/member';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private client = inject(HttpClient);

  getMembers(){
    return this.client.get<Member[]>(environment.baseUrl + 'user');
  }

  getMember(id: string){
    return this.client.get<Member>(environment.baseUrl + 'user/' + id)
  }

  getMemberPhotos(id: string){
    return this.client.get<Photo[]>(environment.baseUrl + 'user/' + id + '/photos');
  }
}
