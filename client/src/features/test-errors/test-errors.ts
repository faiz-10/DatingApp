import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css'
})
export class TestErrors {
  private httpClient = inject(HttpClient);

  baseUrl = 'https://localhost:5001/api/';

  protected validationErrors = signal<string[]>([]);

  get400ValidationError(){
    this.httpClient.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => {
        console.log(error);
        this.validationErrors.set(error);
      } 
    })
  }

  get401Error(){
    this.httpClient.get(this.baseUrl + 'bug/auth').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error(){
    this.httpClient.get(this.baseUrl + 'bug/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400Error(){
    this.httpClient.get(this.baseUrl + 'bug/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get404Error(){
    this.httpClient.get(this.baseUrl + 'bug/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }
}
