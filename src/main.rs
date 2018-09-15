#![feature(plugin)]
#![plugin(rocket_codegen)]

extern crate rocket;

mod handlers;

#[get("/")]
fn index() -> &'static str {
    "Hello, World!"
}

fn main() {
    rocket::ignite()
        .mount("/", routes![index])
        .mount("/zv", routes![handlers::version])
        .launch();
}
