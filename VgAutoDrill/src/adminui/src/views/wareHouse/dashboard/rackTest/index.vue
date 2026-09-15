<template>
  <my-template :deviceKinds="[15]" :formName="'rackTest'" />
</template>

<script>
import MyTemplate from "./template.vue";
export default {
  components: { MyTemplate },
};
</script>
