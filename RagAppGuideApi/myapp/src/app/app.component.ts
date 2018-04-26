
import { Component, OnInit } from '@angular/core';
import{ MyStuffService } from './my-stuff.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent implements OnInit {
  myStuff: any;

  title = 'poo poo';

  constructor(private service : MyStuffService){}

  ngOnInit(){
     console.log('in ngONInit . . .');
     //this.service.getMyStuff().subscribe(x => this.myStuff = x);

  }

}
