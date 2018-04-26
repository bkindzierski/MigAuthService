import { Injectable } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';



@Injectable()
export class MyStuffService {

  
  constructor(private _http : Http) { }

  getMyStuff() : Observable<any>{
    console.log('in get my stuff . . .');
    return this._http.get('/api/UserData/').map(x => x.json());
  }

}
