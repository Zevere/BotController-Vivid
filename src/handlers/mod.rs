extern crate rocket;

#[get("/version")]
pub fn version() -> &'static str {
    "{\"versions\":[\"1\"]}"
}
