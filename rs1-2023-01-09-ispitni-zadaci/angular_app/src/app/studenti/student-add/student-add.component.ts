import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-add',
  templateUrl: './student-add.component.html',
  styleUrls: ['./student-add.component.css']
})
export class StudentAddComponent implements OnInit {
  @Input() ime:string;
  @Input() prezime:string;
  @Input() odabranaOpstina:string;
  @Input() opstine:any;
  @Output() otvori = new EventEmitter<boolean>();
  show = true;
  constructor(private httpKlijent: HttpClient) { }

  ngOnInit(): void {
  }
  zatvori()
  {
    this.show = !this.show;
    this.otvori.emit(this.show);
  }

  spasiPromjene()
  {
    let opstinaId = 0;
    this.opstine.forEach(
      (opstina:any) =>
      {
        if(opstina.opis == this.odabranaOpstina)
          opstinaId = opstina.id;
      }

    )
    let noviStudent =
      {
        ime: this.ime,
        prezime: this.prezime,
        opstinaId: opstinaId
      }
    this.httpKlijent.post(MojConfig.adresa_servera+ "/DodajStudenta", noviStudent, MojConfig.http_opcije()).subscribe(
      () =>
      {
        porukaSuccess("Uspjesno dodan novi student!");
        window.location.reload();
      }
    );
  }

}
