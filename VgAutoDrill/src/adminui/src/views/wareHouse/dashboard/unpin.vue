<template>
  <my-template :deviceKinds="[5]" :formName="'unpin'" />
</template>

<script>
import MyTemplate from "../components/template.vue";
export default {
  components: { MyTemplate },
};
</script>
